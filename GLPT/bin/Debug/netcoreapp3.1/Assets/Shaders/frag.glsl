#version 330

uniform mat4 viewMatrix;
uniform vec3 camera_location;
uniform vec3 color;

in vec2 coord;
out vec4 colorOut;

#include "structs.glsl"
#include "camera.glsl"

float intersectRaySphere(Sphere sphere, Ray ray){
    vec3 oc = ray.origin - sphere.origin;
    float a = dot(ray.direction, ray.direction);
    float b = 2.0 * dot(oc, ray.direction);
    float c = dot(oc, oc) - sphere.radius * sphere.radius;
    float disc = b * b - 4 * a * c;
    if(disc < 0) return -1f;
    return (-b - sqrt(disc)) / 4 * a * c;
}

vec3 getSphereNormal(Sphere sphere, vec3 point){
    return normalize(point - sphere.origin) + vec3(1);
}


void main() {
    Ray ray = getRay(coord.x, coord.y);
    Sphere sphere = Sphere(vec3(0, 0, 5), 2f);
    float dist = intersectRaySphere(sphere, ray);
    if(dist != -1F) {
        colorOut = vec4(getSphereNormal(sphere, ray.origin + ray.direction * dist), 1);
    } else {
        colorOut = vec4(color, 1);
    }
}