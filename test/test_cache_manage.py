import unittest
import random
from ..texture_manage import TextureManage,DefaultTexture

class TestTextureManage():
    def test_tilemap(self):
        manage=TextureManage()
        def a(key,size):
            return 'FLAG'
        def b(key,size):
            return '1FLAG'
        manage.register('asdas',a)
        assert manage.load('asdas',(1,1))=='FLAG'
        manage.register('asdas',b)
        assert manage.load('asdas',(1,1))!='1FLAG'

    def test_default(self):
        manage=DefaultTexture()
        rand=random.Random()
        for loader in dir(manage):
            if loader.startswith('get_'):
                key=loader.replace('get_','')
                size=(rand.randint(1,500),rand.randint(1,500,))
                assert manage.load(key,size).get_size()==size