from vector2 import vector2
import texture_manage

class GameEntity(object):
    def __init__(self, world, position=(0,0), name="Null", tid="default"):
        self.world = world
        self.name = name
        self.tid = tid
        self.position = vector2(position)
        self.id = 0
    def render(self, surface,origin,size, cache=texture_manage.DefaultTexture()):
        texture=cache.load(self.tid,tuple(size))
        surface.blit(texture, origin)
    def update(self, time_passed):
        pass

class Human(GameEntity):
    def __init__(self, world, position=(0,0), name="Null", tid="human"):
        GameEntity.__init__(self, world, position, name, tid)