from tile import Tile
class TileMap(object):
    def __init__(self,size=(30,30)):
        self.size=size
        self.data=[[Tile() for i in range(size[1])] for j in range(size[0])]
    def __getitem__(self,pos):
        return self.data[pos[0]][pos[1]]
