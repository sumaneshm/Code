import time


def make_timer():
    last_called = None

    def elapsed():
        nonlocal last_called
        now = time.time()

        if last_called == None:
            last_called = now
            return None

        result = now - last_called
        last_called = now
        return result

    return elapsed


def experiment1():
    t = make_timer()
    print("First time : ", t())
    print("First time : ", t())
    print("First time : ", t())
    print("First time : ", t())



if __name__ == '__main__':
    experiment1()
