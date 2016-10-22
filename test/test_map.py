import unittest
from ..model import tilemap

class TestTileMap():
    def test_tilemap(self):
        tmap=tilemap.TileMap((50,50))
        for i in range(tmap.size[0]):
            for j in range(tmap.size[1]):
                assert tmap[i,j].type==0
