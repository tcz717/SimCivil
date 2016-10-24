import unittest
from ..model import tilemap

class TestTileMap():
    def test_tilemap(self):
        tmap=tilemap.TileMap((50,50))
        for i in range(tmap.size[0]):
            for j in range(tmap.size[1]):
                assert tmap[i,j].type==0
    def test_get_neighbor(self):
        tmap=tilemap.TileMap((3,3))
        tmap[0,1].blocked=True
        tmap[2,1].blocked=True

        neighbor=tmap.get_neighbors((1,1))
        assert (0,0) not in neighbor
        assert (2,2) not in neighbor
        assert (0,2) not in neighbor
        assert (2,0) not in neighbor
