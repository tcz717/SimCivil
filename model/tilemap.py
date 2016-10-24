class Tile(object):
    def __init__(self,type=0,tid='grass',effciency=1.0,blocked=False):
        self.type=type
        self.tid=tid
        self.effciency=effciency
        self.blocked=False
        self.neighbors=None
        self.marked=0

class TileMap(object):
    def __init__(self,size=(30,30)):
        self.size=size
        self.data=[[Tile() for i in range(size[1])] for j in range(size[0])]
    def __getitem__(self,pos):
        return self.data[pos[0]][pos[1]]
    def __contains__(self,pos):
        x,y=pos
        w,h=self.size
        return 0 <= x and x < w and 0 <= y and y < h
    def get_around(self,pos,ran=1):
        x,y=pos
        for i in xrange(x-ran,x+ran+1):
            for j in xrange(y-ran,y+ran+1):
                if (i,j) in self:
                    yield (i,j)

    def get_neighbors(self,pos):
        tile=self[pos]
        if not tile.neighbors:
            tile.neighbors=[]
            for p in self.get_around(pos):
                if not self[p].blocked:
                    tile.neighbors.append(p)
        return tile.neighbors
            

