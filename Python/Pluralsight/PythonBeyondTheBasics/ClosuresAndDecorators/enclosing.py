def experiment1():
    g = 20
    print("Step 1 : g: {}".format(g))

    def func():
        # as we have redefined a new variable called g inside this function, it is not legal to use g before the
        # the redeclaration and hence the following line would throw compilation error

        # print("Step 2 : g : {}".format(g))

        # if we comment out the next line, the previous line would be valid, i.e. it will refer to the variable
        # declared in the outer space
        g = 20

        print("Step 2 : g : {}".format(g))

    func()
    print("Step 3 : g : {}".format(g))


glb = "Outer"


def experience_message_enclosing():
    global glb
    glb = "Middle"
    mid = "Mid"
    def inner():
        # if we have to refer to global variable, declare that it is global
        global glb
        # nonlocal indicates that the variable is declared outside the scope and don't create a new variable
        nonlocal mid

        glb = "Inner"
        mid = "Inner"

        print("inner : ", glb, " " , mid)

    print("experience_message_enclosing : ", glb, " ", mid)
    inner()
    print("experience_message_enclosing : ", glb, " ", mid)


def experiment2():
    print("experiment2 : ", glb)
    experience_message_enclosing()
    print("experiment2 : ", glb)

if __name__ == '__main__':
    experiment2()
