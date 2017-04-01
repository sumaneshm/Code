import os
# note that we have to refer to the complete package name to include modules
# even if this class is inside that package

from level2.compressed import bzipped
from level2.compressed import gzipped

extension_map = {
    '.bz2': bzipped.opener,
    '.gz': gzipped.opener
}

class Reader:
    def __init__(self, filename):
        # get extension of the filename
        extension = os.path.splitext(filename)[1]

        if os.path.exists(filename):
            print('File {} exists'.format(filename))
        else:
            print('Does not exist')

        # if the extension_map has the extension, use the special opener, otherwise
        # use the normal 'open' method
        opener = extension_map.get(extension, open)

        if os.path.exists(filename):
            print('File {} exists'.format(filename))
            self.f = opener(filename, 'rt')
        else:
            raise FileNotFoundError('File {} not found'.format(filename))

    def close(self):
        self.f.close()

    def read(self):
        return self.f.read()