import pygame
class TextureManage(object):
    block_size=(10,10)
    def __init__(self):
        self.cache={}
        self.loaders={}

    def load(self, key, size):
        if key not in self.loaders:
            raise ValueError("Texture key %s not find."%(key))
        
        if key in self.cache and size in self.cache[key]:
            return self.cache[key][size]
        else:
            if key not in self.cache:
                self.cache[key]={}
            self.cache[key][size]=self.loaders[key](key,size)
            return self.cache[key][size]

    def register(self, key, loader):
        self.loaders[key]=loader

    def __getitem__(self,key):
        return self.load(key,block_size)

class DefaultTexture(TextureManage):
    @staticmethod
    def get_grass(key,size):
        tile= pygame.surface.Surface(size)
        tile.fill((0,255,0))
        return tile

    @staticmethod
    def get_default(key,size):
        w,d=size
        s= pygame.surface.Surface(size)
        pygame.draw.line(s,(255,0,0),(0,0),(w,d))
        pygame.draw.line(s,(255,0,0),(w,0),(0,d))
        return s

    @staticmethod
    def get_human(key,size):
        w,d=size
        s= pygame.surface.Surface(size, flags=pygame.SRCALPHA, depth=32)
        pygame.draw.ellipse(s,(255,255,255),((0,0),size))
        return s

    def __new__(cls,*args,**kwargs):
        if not hasattr(cls,'_inst'):
            cls._inst=TextureManage.__new__(cls,*args,**kwargs)
        return cls._inst

    def __init__(self):
        TextureManage.__init__(self)
        for loader in dir(self):
            if loader.startswith('get_'):
                key=loader.replace('get_','')
                self.loaders[key]=self.__getattribute__(loader)
