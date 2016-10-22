from model import tilemap
import pygame
import numpy as np

class MapRender():
    def __init__(self,tmap,texture,block_size=(10,10),space=2):
        self.tilemap=tmap
        self.block_size=np.array(block_size)
        self.space=np.array((space,space))
        self.texture=texture

    def render_map(self, screen,start_pos=(0,0)):
        tiles= pygame.surface.Surface(np.array(self.block_size+self.space)*self.tilemap.size)
        for i in range(self.tilemap.size[0]):
            for j in range(self.tilemap.size[1]):
                tile=self.tilemap[i,j]
                pos =np.array(self.block_size+self.space)*(i,j)
                tiles.blit(self.texture.load(tile.tid,tuple(self.block_size)),pos)
                
        screen.blit(tiles,start_pos)

    def render_entity(self,screen,entity,start_pos=(0,0)):
        pos =np.array(self.block_size+self.space)*entity.position
        entity.render(screen,pos,self.block_size)
