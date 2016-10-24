from common import vector2,enum
import texture_manage
import math

class GameEntity(object):
    def __init__(self, world, position=(0,0), name="Null", tid="default"):
        self.world = world
        self.name = name
        self.tid = tid
        self.precise_position = vector2(position)
        self.id = 0
        self.status={}

    @property
    def position(self):
        x,y=self.precise_position
        return vector2((int(x),int(y)))

    def get_tile(self):
        return self.world[self.position]

    def render(self, surface,origin,size, cache=texture_manage.DefaultTexture()):
        texture=cache.load(self.tid,tuple(size))
        surface.blit(texture, origin)
    def update(self, time_passed):
        pass
    def __getitem__(self,key):
        return self.status[key]
    def __setitem__(self,key,value):
        self.status[key]=value

HumanStatus=enum(
    heath=0,
    walk_speed=1,
    hunger=2,
    vision_range=3,
    position=4,
)
class Human(GameEntity):
    def __init__(self, world, position=(0,0), name="Null", tid="human"):
        GameEntity.__init__(self, world, position, name, tid)
        self.status = {key:0 for key in HumanStatus.all_enums}
        self.__speed=0
        self.__target=position
        self[HumanStatus.position]=position
        self.behavior=None

    def update(self, time_passed):
        x,y=self[HumanStatus.position]
        dir=vector2(self.__target)-self.precise_position
        dx,dy=dir
        norm=math.sqrt(dx*dx+dy*dy)
        if norm>0:
            dl=time_passed*self.__speed
            if norm<dl:
                dl=norm
            dir=dir/norm*dl
            x,y=(x,y)+dir
            self[HumanStatus.position]=(x,y)
            self.precise_position=(x,y)

        if self.behavior:
            self.behavior.update(time_passed)

    def move(self,target,speed):
        self.__speed=speed
        self.__target=target
        dir=vector2(self.__target)-self.precise_position
        dx,dy=dir
        norm=math.sqrt(dx*dx+dy*dy)
        return norm<0.01 and self.__target==tuple(self.position)