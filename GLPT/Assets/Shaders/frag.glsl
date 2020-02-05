#version 330

#include "header.glsl"

#define SPHERE_COUNT %SCENE_SPHERE_COUNT%
#define CHECK_SPHERE %SCENE_CHECK_SPHERE%
#define PLANE_COUNT %SCENE_PLANE_COUNT%
#define CHECK_PLANE %SCENE_CHECK_PLANE%
uniform Sphere spheres[SPHERE_COUNT];
uniform Plane planes[PLANE_COUNT];

vec3 getSphereNormal(Sphere sphere, vec3 point){
    return normalize(point - sphere.origin) + vec3(1);
}

vec3 planeCheckerboard(vec3 point){
    float scale = 4;
    int chess = int(floor(point.x / scale) + floor(point.z /scale));
    if(chess % 2 == 0){
        return vec3(255);
    } else {
        return vec3(0);
    }
}

vec3 trace(Ray ray){
    float closestSphereHit = -1F;
    int closestSphereIndex = -1;
    for(int i=0; i<spheres.length && CHECK_SPHERE; i++){ 
        float t = intersectRaySphere(ray, spheres[i]);
        if(t != -1F && (t < closestSphereHit || closestSphereHit == -1F)) { // There is a ray sphere intersection
            closestSphereHit = t;
            closestSphereIndex = i;
        }
    }
    
    float closestPlaneHit = -1F;
    int closestPlaneIndex = -1;
    for(int i=0; i<planes.length && CHECK_PLANE; i++){
        float t = intersectRayPlane(ray, planes[i]);
        if(t != -1F && (t < closestPlaneHit || closestPlaneHit == -1F)){
            closestPlaneHit = t;
            closestPlaneIndex = i;
        }
    }
    
    if(closestPlaneHit == -1F && closestSphereHit != -1F){
        return closestSphereIndex == -1F ? color : getSphereNormal(spheres[closestSphereIndex], ray.origin + ray.direction * closestSphereHit);
    }   
    if(closestPlaneHit != -1F && closestSphereHit == -1F){
        return closestPlaneIndex == -1F ? color : planeCheckerboard(ray.origin + ray.direction * closestPlaneHit);
    }
    if(closestSphereHit != -1F && closestPlaneHit != -1F){
        if(closestSphereHit > closestPlaneHit){
            return closestSphereIndex == -1F ? color : getSphereNormal(spheres[closestSphereIndex], ray.origin + ray.direction * closestSphereHit);
        } else {
            return closestPlaneIndex == -1F ? color : planeCheckerboard(ray.origin + ray.direction * closestPlaneHit);
        }
    }
    return color;
    
}

void main() {
    Ray ray = getRay(coord.x, coord.y);
    colorOut = vec4(trace(ray), 1);
}