from model import tilemap
import pygame
from common import vector2

class MapRender():
    def __init__(self,tmap,texture,block_size=(10,10),space=2):
        self.tilemap=tmap
        self.block_size=vector2(block_size)
        self.space=vector2(space,space)
        self.texture=texture
        self.map_cache=None

    def render_map(self, screen,start_pos=(0,0)):
        if not self.map_cache:
            tiles= pygame.surface.Surface(vector2(self.block_size+self.space)*self.tilemap.size)
            for i in range(self.tilemap.size[0]):
                for j in range(self.tilemap.size[1]):
                    tile=self.tilemap[i,j]
                    pos =vector2(self.block_size+self.space)*(i,j)
                    tiles.blit(self.texture.load(tile.tid,tuple(self.block_size)),pos)
            self.map_cache=tiles
                
        screen.blit(self.map_cache,start_pos)

    def render_entity(self,screen,entity,start_pos=(0,0)):
        pos =vector2(self.block_size+self.space)*entity.precise_position
        entity.render(screen,pos,self.block_size)
