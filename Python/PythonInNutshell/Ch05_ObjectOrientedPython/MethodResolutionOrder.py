__author__ = 'sumaneshm'

#Method resolution order is as follows
# 1. Checks whether the current instance's class supports the method
# 2. Goes to the base classes of the current class from left to right in the order they are declared and checks whether
#    supports the method
# 3. If not, goes to the left most class's base classes from the left to right mode and tries to resolve

#Example 1

class E1_Base_L:
    def method(self): print ("E1_Base_L")

class E1_Base_R:
    def method(self): print ("E1_Base_R")

class E1_Derived(E1_Base_L, E1_Base_R): pass
class E1_ReverseDerived(E1_Base_R, E1_Base_L): pass


ex1_1 = E1_Derived()
ex1_2 = E1_ReverseDerived()
print("Example 1 : ")
ex1_1.method()            #Will print E1_Base_L
ex1_2.method()            #Will print E1_Base_R

print(E1_Derived.__mro__)   #__mro__ will print the class's method resolution order in detail for easy understanding

#Example 2
class Base1:
    def amethod(self): print("Base1")

class Base2: pass
class Base3:
    def amethod(self): print("Base3")

class Derived(Base2, Base3):pass

print("\nExample 2 : ")
d = Derived()
d.amethod()         #Will print Base3


print(Derived.__mro__)
