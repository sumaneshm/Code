from pprint import pprint as pp

class A:
    def func(self):
        print("A.func")


class B(A):
    def func(self):
        print("B.func")


class C(B):
    def func(self):
        print("C.func")


class D(C):
    def func(self):
        print("D.func")


def experiment1():
    d = D()
    print(D.mro())

    ###### class bound proxy
    # super can be called on class
    # 1. super(super-class, sub-class) syntax
    # 2. first D's MRO will be called
    # 3. whatever class listed in MRO 'after' the "super-class" declared will be considered
    # 4. the first class which defines this function in the resultant list will be called

    # MRO for sub-class D is
    # D, C, B, A, object
    # here we have set super-class as "C", so B which comes after C will be considered first
    super(C, D).func(d)

    ##### instance based super proxy
    # similar to class bound proxy, here we can use an instance in place of sub-class
    # rest same... :D

    super(B, d).func()

class foo:
    def bar(self):
        print("foo.bar")

class subfoo(foo):
    def bar(self):
        # when super is called without any parameters, python assumes the following parameter
        # super(class-of-the-method, self)
        # i.e. in this case, the following is equivalent to
        # super(subfoo, self)
        super().bar()
        print("subfoo.bar")


def experiment2():
    s = subfoo()
    s.bar()

class O:
    def func(self):
        print("O.func")

class A(O):
    def func(self):
        super().func()
        print("A.func")

class B(O):
    def func(self):
        super().func()
        print("B.func")

class AB(A, B):
    def func(self):
        super().func()
        print("AB.func")

def experiment3():
    # 1. when both the parents of AB i.e. A and B, are inheriting from the same common ancestor O,
    # 2. when both the parents A, B call their super().func
    # 3. calling ab.func will call both A.func, B.func (which in turn call O.func) respectively
    ab = AB()
    pp(AB.mro())
    ab.func()

if __name__ == '__main__':
    experiment3()
