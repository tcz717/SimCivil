import unittest
from ..render.maprender import MapRender
from ..model.tilemap import TileMap

class TestMapRender():
    def test_maprender(self):
        render=MapRender(TileMap(),None)
        assert True
