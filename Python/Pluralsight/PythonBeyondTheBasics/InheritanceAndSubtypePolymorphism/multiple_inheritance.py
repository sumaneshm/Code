class Base1:
    def __init__(self):
        print("Base1 init")

class Base2:
    def __init__(self):
        print("Base2 init")


class Derived1(Base1, Base2):
    pass

# Rules
# When a class inherits from multiple classes and doesn't have explicit constructor, first base class (defined first)
# will be called

def experiment1():
    d = Derived1()
    print(Derived1.__bases__)   # __bases__ on the class will give all the base classes as a tuple


if __name__ == '__main__':
    experiment1()
