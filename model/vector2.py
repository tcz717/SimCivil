import numpy as np

def vector2(*xy):
    if len(xy)==1:
        return np.array(xy[0])
    elif len(xy)==2:
        return np.array(xy)
    else:
        raise ValueError(str(xy)+"is not vector2")
