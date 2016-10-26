from model import tilemap
import pygame
from common import vector2

class MapRender():
    def __init__(self,tmap,texture,screen_size,block_size=(10,10),space=2):
        self.tilemap=tmap
        self.block_size=vector2(block_size)
        self.space=vector2(space,space)
        self.texture=texture
        self.map_cache=None
        self.screen_size=vector2(screen_size) 
        self.__last_pos=vector2(0,0)

    @property
    def unit_size(self):
        return self.block_size+self.space
    @property
    def actual_size(self):
        return self.unit_size*self.tilemap.size

    def _draw_all(self,start_pos):
        self._draw_part(vector2(0,0),self.screen_size,start_pos)
        print 'redraw'

    def _draw_part(self,lt,rb,start_pos):
        self.map_cache.fill((0,0,0),rect=pygame.rect.Rect(lt,rb-lt))
        lt=self.screen2map(lt,start_pos)
        rb=self.screen2map(rb,start_pos)
        for i in range(lt.x,rb.x+1):
            for j in range(lt.y,rb.y+1):
                if (i,j) in self.tilemap:
                    tile=self.tilemap[i,j]
                    pos =self.map2screen((i,j),start_pos)
                    self.map_cache.blit(self.texture.load(tile.tid,self.block_size),pos)
        

    def render_map(self, screen,start_pos):
        diff=start_pos-self.__last_pos
        if not self.map_cache:
            self.map_cache=pygame.surface.Surface(self.screen_size)
            self._draw_all(start_pos)
        elif abs(diff.x)>self.screen_size.x or abs(diff.y)>self.screen_size.y:
            self._draw_all(start_pos)
        elif diff.length==0:
            pass
        else:
            self.map_cache.scroll(-diff.x,-diff.y)
            if diff.x>0:
                p1a=vector2(self.screen_size.x-diff.x,0)
                p1b=self.screen_size
                if diff.y>0:
                    p2a=vector2(0,self.screen_size.y-diff.y)
                    p2b=vector2(self.screen_size.x-diff.x,self.screen_size.y)
                else:
                    p2a=vector2(0,0)
                    p2b=vector2(self.screen_size.x-diff.x,-diff.y)
            else:
                p1a=vector2(0,0)
                p1b=vector2(-diff.x,self.screen_size.y)
                if diff.y>0:
                    p2a=vector2(-diff.x,self.screen_size.y-diff.y)
                    p2b=self.screen_size
                else:
                    p2a=vector2(-diff.x,0)
                    p2b=vector2(self.screen_size.x,-diff.y)
            # print diff,p1a,p1b,p2a,p2b
            self._draw_part(p1a,p1b,start_pos)
            self._draw_part(p2a,p2b,start_pos)
         
        self.__last_pos=vector2(start_pos) 
        screen.blit(self.map_cache,(0,0))

    def render_entity(self,screen,entity,start_pos=(0,0)):
        pos =self.map2screen(entity.precise_position,start_pos)
        entity.render(screen,pos,self.block_size)

    def screen2map(self,screen_pos,start_pos=None):
        start_pos= start_pos if start_pos else self.__last_pos
        return ((screen_pos+start_pos)/self.unit_size).cast(int)

    def map2screen(self,map_pos,start_pos=None):
        start_pos= start_pos if start_pos else self.__last_pos
        return map_pos*self.unit_size-start_pos

    def set_screen_size(self,size):
        self.screen_size=size
        self.map_cache=None
