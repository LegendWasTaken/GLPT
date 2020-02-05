#version 330

#include "header.glsl"

#define SPHERE_COUNT %SCENE_SPHERE_COUNT%
uniform Sphere spheres[SPHERE_COUNT];

vec3 getSphereNormal(Sphere sphere, vec3 point){
    return normalize(point - sphere.origin) + vec3(1);
}

vec3 trace(Ray ray){
    float closestSphereHit = -1F;
    int closestSphereIndex = -1;
    for(int i=0; i<spheres.length; i++){ 
        float t = intersectRaySphere(ray, spheres[i]);
        if(t != -1F && (t < closestSphereHit || closestSphereHit == -1F)) { // There is a ray sphere intersection
            closestSphereHit = t;
            closestSphereIndex = i;
        }
    }
    return closestSphereIndex == -1F ? color : getSphereNormal(spheres[closestSphereIndex], ray.origin + ray.direction * closestSphereHit);
}

void main() {
    Ray ray = getRay(coord.x, coord.y);
    colorOut = vec4(trace(ray), 1);
}