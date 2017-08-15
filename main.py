#!/usr/bin/env python
import pygame
import texture_manage
from render.maprender import MapRender
from model.tilemap import TileMap
from model.entity import Human,HumanStatus
from model.behavior import BehaviorInterface
from generator import MapGenerator,EntityGenerator
from common import vector2
from pygame.locals import *
from sys import exit
pygame.init()

size=(640, 480)
map_position=vector2(0,0)
screen = pygame.display.set_mode(size, 0, 32)
pygame.display.set_caption("Sim Civil")
fontobj = pygame.font.SysFont('arial', 15)

mgen=MapGenerator()
tmap=mgen.generate((100,100))
cache=texture_manage.DefaultTexture()
render=MapRender(tmap,cache,size,block_size=(15,15))

egen=EntityGenerator(tmap)
man=egen.get_std_human("test")
behavior=man.behavior

clock=pygame.time.Clock()
while True:
    tx,ty=render.screen2map(pygame.mouse.get_pos())
    for event in pygame.event.get():
        if event.type == QUIT:
            exit()
        if event.type == MOUSEBUTTONDOWN:
            if pygame.mouse.get_pressed()[0]:
                behavior.move((tx,ty))

    pressed_keys = pygame.key.get_pressed()
    rel=pygame.mouse.get_rel()
    if pygame.mouse.get_pressed()[2]:
        map_position=map_position-rel
    if pressed_keys[K_LEFT]:
        map_position.x = map_position.x-5
    elif pressed_keys[K_RIGHT]:
        map_position.x = map_position.x+5
    if pressed_keys[K_UP]:
        map_position.y = map_position.y-5
    elif pressed_keys[K_DOWN]:
        map_position.y = map_position.y+5

    screen.fill((0, 0, 0))

    time_passed=clock.tick(50)/1000.0

    man.update(time_passed)
    if time_passed>0.1:
        print time_passed

    render.render_map(screen,map_position)
    render.render_entity(screen,man,map_position)
    screen.blit(fontobj.render(
        str(clock.get_fps())+' '+str(man.position)+' '+str(map_position),
        True,
        (255,0,0)
        ),(0,0))
    if (tx,ty) in tmap:
        screen.blit(fontobj.render(
            str((tx,ty))+' '+str(tmap[tx,ty].tid),
            True,
            (255,0,0)
            ),(0,15))

    pygame.display.update()