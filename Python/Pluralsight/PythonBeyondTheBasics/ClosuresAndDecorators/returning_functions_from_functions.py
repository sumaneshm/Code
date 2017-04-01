
def experiment1():
    square = get_exponential(2)
    cube = get_exponential(3)

    print(square(2))
    print(cube(2))

# here get_exponential is called as function factory as it generates functions
# we are creating a closure here by referring to variables/params in outer scope inside the local functions
def get_exponential(times):
    def expo(n):
        return n ** times
    # dunder closure is a special variable name which lists all the objects that it closes (captures)

    print (expo.__closure__)

    return expo

if __name__ == '__main__':
    experiment1()