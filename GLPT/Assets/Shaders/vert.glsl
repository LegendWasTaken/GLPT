#version 330

uniform vec2 res;

in vec4 position;
in vec2 texCoord;

out vec2 coord;

void main() {
    coord = (position.xy + 1) / 2;
    gl_Position = position;
}