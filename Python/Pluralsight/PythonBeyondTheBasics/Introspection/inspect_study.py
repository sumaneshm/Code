import inspect
from ImplementingCollections import sorted_set
from pprint import pprint as pp

def experiment1():
    print(inspect.ismodule(sorted_set))
    # pp(inspect.getmembers(sorted_set))
    pp(inspect.getmembers(sorted_set, inspect.isclass))

if __name__ == '__main__':
    experiment1()