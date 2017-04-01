import contextlib


@contextlib.contextmanager
def manager(name):
    print("Manager : {} - Start ".format(name))
    try:
        yield name
        print("Manager : {} - Back normally".format(name))
    except:
        print("Manager : {} - Exception case")


def experiment1():
    # the following two methods are equal
    with manager("Sumanesh") as hubby, manager("Saveetha") as wife:
        print(hubby)
        print(wife)

    print("\n\n")
    with manager("Sumanesh") as hubby:
        with manager("Saveetha") as wife:
            print(hubby)
            print(wife)


def experiment2():
    #  input from first can be passed to second
    with manager("Sumanesh") as h, manager("Saveetha married to " + h):
        pass



if __name__ == '__main__':
    experiment2()