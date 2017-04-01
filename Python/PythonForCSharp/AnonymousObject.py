__author__ = 'sumaneshm'

class AnonObject(dict):
    __getattr__ = dict.get
    __setattr__ = dict.__setattr__



def main():
    anon = AnonObject(id=7,registered=False,name="Sumanesh")

    if(anon.registered):
        print('{0} is already registered'.format(anon.name))
    else:
        print('{0} is yet to register'.format(anon.name))


if (__name__=="__main__"):
    main()