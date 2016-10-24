import random
import noise
from model.tile import *
from model.tilemap import *

class TileGenerator(object):
    @staticmethod
    def get_grass():
        tile=Tile(
            type=0,
            tid='grass',
            effciency=0.8,
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

class MapGenerator(object):
    def __init__(self,size):
        self.size=size
        self.max=10000

    def generate(self,seed=None):
        rand=random.Random(seed) if seed else random.Random()
        w,h=self.size
        scale=20.0
        tmap=TileMap(self.size)
        hx=rand.randint(-self.max,self.max)
        hy=rand.randint(-self.max,self.max)
        rx=rand.randint(-self.max,self.max)
        ry=rand.randint(-self.max,self.max)
        for i in range(w):
            for j in range(h):
                height=noise.pnoise2((i+hx)/scale,(j+hy)/scale,octaves=4,persistence=0.25)*3+0.5
                rain=noise.pnoise2((i+rx)/scale,(j+ry)/scale,octaves=4,persistence=0.25)
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
