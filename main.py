#!/usr/bin/env python
import pygame
import texture_manage
from render.maprender import MapRender
from model.tilemap import TileMap
from model.entity import Human
from pygame.locals import *
from sys import exit
pygame.init()

screen = pygame.display.set_mode((640, 480), 0, 32)
pygame.display.set_caption("Sim Stick")

tmap=TileMap((30,30))
cache=texture_manage.DefaultTexture()
render=MapRender(tmap,cache)
man = Human(tmap,(25,25),"test")

while True:
    for event in pygame.event.get():
        if event.type == QUIT:
            exit()
    screen.fill((0, 0, 0))
    render.render_map(screen)
    render.render_entity(screen,man)

    pygame.display.update()