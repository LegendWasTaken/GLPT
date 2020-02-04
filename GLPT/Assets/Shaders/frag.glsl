#version 330

uniform vec2 res;
uniform mat4 viewMatrix;

in vec2 coord;
out vec4 colorOut;

struct Sphere {
    vec3 origin;
    float radius;
};

struct Ray {
    vec3 origin;
    vec3 direction;
};

Ray getRay(float x, float y){
    vec3 pos = vec3(viewMatrix * vec4(0, 0, 0, 1));
    vec3 dir = vec3(transpose(inverse(mat3(viewMatrix))) * vec3(x, y, 1));
    return Ray(pos, normalize(dir));
}

vec3 getSphereNormal(Sphere sphere, vec3 point){
    vec3 sphereNormal = (normalize(point - sphere.origin) + vec3(1)) / 2;
    return sphereNormal;
}

float hitSphere(Sphere sphere, Ray ray){
    vec3 oc = ray.origin - sphere.origin;
    float a = dot(ray.direction, ray.direction);
    float b = 2.0 * dot(oc, ray.direction);
    float c = dot(oc, oc) - sphere.radius * sphere.radius;
    float disc = b * b - 4 * a * c;
    if(disc < 0) return -1f;
    return (-b - sqrt(disc)) / 4 * a * c;
}

void main() {
    Sphere sphere = Sphere(vec3(0, 0, 5), 3);
    vec3 pixel = vec3(.27F, .27F, .27F);
    Ray ray = getRay(coord.x * 2 - 1, coord.y * 2 - 1);
    float dist = hitSphere(sphere, ray);
    if(dist > 0F) {
//        pixel = vec3(255, 0, 0);
        pixel = getSphereNormal(sphere, ray.origin + ray.direction * dist);
    }
	colorOut = vec4(pixel, 1);
}