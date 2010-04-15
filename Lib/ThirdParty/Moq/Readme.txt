Some of the Enterprise Library unit tests use features of the Moq mock object library.
We cannot include these libraries as part of the Enterprise Library download. To run
the unit tests, please download the binaries for Moq 3.1 from the
project site at:

http://code.google.com/p/moq/

(current version as of this writing is 3.1.416.3)

and unpack the binaries and place them in this directory. Both the desktop
and Silverlight binaries can be placed here. You will then be able to compile
and run the unit tests.

