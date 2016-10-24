import numpy as np

def vector2(*xy):
    if len(xy)==1:
        return np.array(xy[0])
    elif len(xy)==2:
        return np.array(xy)
    else:
        raise ValueError(str(xy)+"is not vector2")


def enum(*sequential, **named):
    enums = dict(zip(sequential, range(len(sequential))), **named)
    reverse = dict((value, key) for key, value in enums.iteritems())
    enums['all_enums'] = list(key for key, value in enums.iteritems())
    enums['reverse_mapping'] = reverse
    return type('Enum', (), enums)
