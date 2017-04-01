__author__ = 'sumaneshm'

# If we need to call the base class method(including constructor), we have to EXPLICITLY call that

class E1_Base1(object):
    def method(self):
        print("E1_Base1")

class E1_Base2(E1_Base1):
    def method(self):
        print("E1_Base2")
        E1_Base1.method(self)

class E1_Base3(E1_Base1):
    def method(self):
        print("E1_Base3")
        E1_Base1.method(self)

class E1_Derived(E1_Base2, E1_Base3):
    def method(self):
        print("Derived")
        E1_Base2.method(self)
        E1_Base3.method(self)

d = E1_Derived()
d.method()

#In Example 1, we are explicitly calling the base method using the base class names, and this would mean that base most
# base class will be called twice and refering to the immediate base class as "super" will resolve this issue

#Example 2 -----------------------------------------------
print("\nExample 2")
class E2_Base1(object):
    def method(self):
        print("E2_Base1")

class E2_Base2(E2_Base1):
    def method(self):
        print("E2_Base2")
        super(E2_Base2, self).method()

class E2_Base3(E2_Base1):
    def method(self):
        print("E2_Base3")
        super(E2_Base3, self).method()

class E2_Derived(E2_Base2, E2_Base3):
    def method(self):
        print("Derived")
        super(E2_Derived, self).method()

d = E2_Derived()
d.method()

#In Example 2, we are calling super class implicitly as "super" and hence it won't call base most class's method twice

