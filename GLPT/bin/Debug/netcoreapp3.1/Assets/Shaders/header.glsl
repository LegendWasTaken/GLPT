uniform vec2 res;
uniform vec3 camera_location;
uniform vec3 color;
uniform mat4 viewMatrix;
in vec2 coord;
out vec4 colorOut;
#include "structs.glsl"
#include "camera.glsl"
#include "intersections.glsl"
