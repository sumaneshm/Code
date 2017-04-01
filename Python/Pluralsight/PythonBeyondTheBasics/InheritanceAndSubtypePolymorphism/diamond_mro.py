class A:
    def func(self):
        print("A.func")

class B(A):
    def func(self):
        print("B.func")

class C(A):
    def func(self):
        print("C.func")

class D1(B, C):
    pass


def experiment1():
    # MRO - Methor Resolution order lists the order in which classes will be looked for
    # when a method is called.
    # In this example the MRO would be : D1, B, C, A, object
    # (B comes first as the D1 declaration first inherits from B)
    print(D1.mro())

    d = D1()
    d.func() # B's func method will be called

if __name__ == '__main__':
    experiment1()