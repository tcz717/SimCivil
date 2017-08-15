import random
import sys
import noise
from model.behavior import BehaviorInterface
from model.entity import *
from model.tile import *
from model.tilemap import *

# heath=0,
# walk_speed=1,
# hunger=2,
# vision_range=3,
# position=4,
STDWALKSPEED=2.0

class TileGenerator(object):
    @staticmethod
    def get_grass():
        tile=Tile(
            type=0,
            tid='grass',
            effciency=0.9,
            blocked=False
        )
        return tile
    @staticmethod
    def get_dirty():
        tile=Tile(
            type=1,
            tid='dirty',
            effciency=1.0,
            blocked=False
        )
        return tile
    @staticmethod
    def get_stone():
        tile=Tile(
            type=2,
            tid='stone',
            effciency=0.1,
            blocked=True
        )
        return tile
    @staticmethod
    def get_shallow():
        tile=Tile(
            type=3,
            tid='shallow',
            effciency=0.5,
            blocked=False
        )
        return tile
    @staticmethod
    def get_water():
        tile=Tile(
            type=4,
            tid='water',
            effciency=0.1,
            blocked=True
        )
        return tile

class EntityGenerator(object):
    def __init__(self,tmap):
        self.tmap=tmap
    def get_std_human(self,name=None,position=None):
        rand=random.Random()
        if not position:
            position=(0,0)
            w,h=self.tmap.size
            for i in xrange(w*h):
                x,y=rand.randint(0,w-1),rand.randint(0,h-1)
                if not self.tmap[x,y].blocked:
                        position=(x,y)
                        break

        man = Human(self.tmap,position,name)
        man.behavior=BehaviorInterface(man)
        man[HumanStatus.walk_speed]=STDWALKSPEED
        return man

class MapGenerator(object):
    def __init__(self,seed=None):
        self.max=10000
        if seed:
            self.rand=random.Random(seed)
        else:
            rand=random.Random()
            self.seed=rand.randint(-sys.maxint+1,sys.maxint)
            self.rand=random.Random(self.seed)

        self.hx=self.rand.randint(-self.max,self.max)
        self.hy=self.rand.randint(-self.max,self.max)
        self.rx=self.rand.randint(-self.max,self.max)
        self.ry=self.rand.randint(-self.max,self.max)

    def generate(self,size,offset=(0,0)):
        w,h=size
        scale=20.0
        tmap=TileMap(size)

        tmap.height_center=(self.hx,self.hy)
        tmap.rain_center=(self.rx,self.ry)

        for i in range(w):
            for j in range(h):
                height=noise.pnoise2((i+self.hx)/scale,(j+self.hy)/scale,octaves=4,persistence=0.25)*3+0.5
                rain=noise.pnoise2((i+self.rx)/scale,(j+self.ry)/scale,octaves=4,persistence=0.25)
                if height>1.2:
                    tile=TileGenerator.get_stone()
                elif height<-0.5:
                    tile=TileGenerator.get_water()
                elif height<0 or rain>0.5:
                    tile=TileGenerator.get_shallow()
                elif height>1:
                    tile=TileGenerator.get_dirty()
                elif height>0.6 and rain < 0:
                    tile=TileGenerator.get_dirty()
                else:
                    tile=TileGenerator.get_grass()
                tmap.data[i][j]=tile
        return tmap
