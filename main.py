#!/usr/bin/env python
import pygame
import texture_manage
from render.maprender import MapRender
from model.tilemap import TileMap
from model.entity import Human,HumanStatus
from model.behavior import BehaviorInterface
from generator import MapGenerator
from pygame.locals import *
from sys import exit
pygame.init()

screen = pygame.display.set_mode((640, 480), 0, 32)
pygame.display.set_caption("Sim Stick")
fontobj = pygame.font.SysFont('arial', 15)

mgen=MapGenerator((30,30))
tmap=mgen.generate()
cache=texture_manage.DefaultTexture()
render=MapRender(tmap,cache,block_size=(15,15))

man = Human(tmap,(9,8),"test")
behavior=BehaviorInterface(man)
man.behavior=behavior
man[HumanStatus.walk_speed]=2.0

clock=pygame.time.Clock()
while True:
    tx,ty=pygame.mouse.get_pos()
    tx,ty=tx/(render.block_size[0]+render.space[0]),ty/(render.block_size[1]+render.space[1])
    for event in pygame.event.get():
        if event.type == QUIT:
            exit()
        if event.type == MOUSEBUTTONDOWN:
            behavior.move((tx,ty))
    screen.fill((0, 0, 0))

    time_passed=clock.tick(50)/1000.0

    man.update(time_passed)
    if time_passed>0.1:
        print time_passed

    render.render_map(screen)
    render.render_entity(screen,man)
    screen.blit(fontobj.render(
        str(int(1/time_passed))+' '+str(man.position),
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