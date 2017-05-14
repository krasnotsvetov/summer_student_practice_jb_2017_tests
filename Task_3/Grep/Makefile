TARGET = grep
CC = g++
CFLAGS = -std=c++11
LIBS = -lstdc++fs -pthread

.PHONY: default all clean

default: $(TARGET)
all: default

OBJECTS = $(patsubst %.cpp, %.o, $(wildcard Grep/*.cpp))
HEADERS = $(wildcard Grep/*.h)

%.o : %.cpp $(HEADERS)
	$(CC) $(CFLAGS) -c $< -o $@

.PRECIOUS: $(TARGET) $(OBJECTS)

$(TARGET): $(OBJECTS)
	$(CC) $(OBJECTS) $(CFLAGS) -o $@ $(LIBS)

clean:
	-rm -f Grep/*.o
	-rm -f Grep$(TARGET)
