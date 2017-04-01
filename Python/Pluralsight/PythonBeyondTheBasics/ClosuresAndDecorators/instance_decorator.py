class Tracer:
    def __init__(self):
        self.trace_enabled = True

    def __call__(self, f):
        def wrap(*args, **kwargs):
            if self.trace_enabled:
                print("---> Calling ", f)
            r = f(*args, **kwargs)
            if self.trace_enabled:
                print("--->Result is : ", r)
            return r
        return wrap


tracer = Tracer()


def experiment1():
    print("Trace enabled")
    callee()
    tracer.trace_enabled = False
    print("Trace disabled")
    callee()


@tracer
def callee():
    print("Function called")


if __name__ == '__main__':
    experiment1()