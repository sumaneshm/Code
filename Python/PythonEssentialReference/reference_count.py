import sys


def experiment1():
    a = 10
    print(sys.getrefcount(a))
    b = 10
    print(sys.getrefcount(a))
    c = a
    print(sys.getrefcount(a))

if __name__ == '__main__':
    experiment1()

