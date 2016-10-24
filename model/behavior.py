from common import enum
from ai.move import astar
from entity import HumanStatus

BehaviorResult=enum(
    'inited',
    'failed',
    'running',
    'finished',
    'canceled',
)

class BehaviorHandle(object):
    def __init__(self,entity,state=BehaviorResult.inited):
        self.state=state
        self.cancel=False
        self.entity=entity

    def handler(self,time_passed):
        return True

class IdleHandle(BehaviorHandle): 
    def __init__(self,entity,timespan=1,state=BehaviorResult.running):
        BehaviorHandle.__init__(self,entity,state)
        self.timespan=timespan

    def handler(self,time_passed):
        self.state=BehaviorResult.running
        if self.timespan==None:
            return
        elif self.timespan>0:
            self.timespan=self.timespan-time_passed
        else:
            self.state=BehaviorResult.finished

class MoveHandle(BehaviorHandle): 
    def __init__(self,entity,target,state=BehaviorResult.inited):
        BehaviorHandle.__init__(self,entity,state)
        self.target=target
        self.limit=500
        self.index=0

    def calculate_path(self):
        def cost(from_pos, to_pos):
            from_y, from_x = from_pos
            to_y, to_x = to_pos
            return (1.4 if to_y - from_y and to_x - from_x else 1.0) / self.entity.world[to_pos].effciency
        def estimate(pos):
            y, x = pos
            goal_y, goal_x = self.target
            dy, dx = abs(goal_y - y), abs(goal_x - x)
            return min(dy, dx) * 1.4 + abs(dy - dx) * 1.0
        self.path=astar(
            tuple(self.entity.position),
            self.entity.world.get_neighbors,
            lambda target:self.target==target,
            0,
            cost,
            estimate,
            self.limit
        )
        self.state=BehaviorResult.running

    def handler(self,time_passed):
        if self.cancel:
            self.state=BehaviorResult.canceled
            return 
        if tuple(self.entity.position)==self.target:
            self.state=BehaviorResult.finished
            return
        if not self.index<len(self.path):
            self.state=self.state=BehaviorResult.finished
            return

        node=self.path[self.index]
        cur_node=self.entity.world[self.entity.position]
        speed=self.entity[HumanStatus.walk_speed]*cur_node.effciency
        if self.entity.move(node,speed):
            self.index=self.index+1
            self.state=BehaviorResult.running
        
class BehaviorInterface(object):
    def __init__(self,entity):
        self.current=IdleHandle(entity)
        self.entity=entity

    def move(self,position,**args):
        self.current=MoveHandle(self.entity,position)
        self.current.calculate_path()
        return self.current
    def idle(self,timespan=1,**args):
        return IdleHandle(self.entity,timespan)
    def use(self,object,**args):
        return BehaviorHandle()
    def interact(self,object,**args):
        return BehaviorHandle()
    def pick(self,object,**args):
        return BehaviorHandle()
    def talk(self,target,**args):
        return BehaviorHandle()

    def get_bag(self,**args):
        return []
    def get_around(self,**args):
        return []
    def get_status(self,**args):
        return dict(self.entity.status)

    def update(self,time_passed):
        if self.current:
            self.current.handler(time_passed)
            if self.current.state!=BehaviorResult.running:
                self.current=None
    