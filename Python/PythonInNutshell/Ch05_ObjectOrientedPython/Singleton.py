__author__ = 'sumaneshm'

class Singleton(object):
    _singletons = {}

    def __new__(cls, *args, **kwargs):
        if(cls not in cls._singletons):
            cls._singletons[cls] = super(Singleton, cls).__new__(cls)

s1 = Singleton()
s2 = Singleton()

print("Singleton instance check ", s1 == s2)

class Student: pass

s1 = Student()
s2 = Student()

print ("Non singleton instance check : ", s1 == s2)
