// Ray Sphere intersection
float intersectRaySphere(Ray ray, Sphere sphere){
    vec3 oc = ray.origin - sphere.origin;
    float a = dot(ray.direction, ray.direction);
    float b = 2.0 * dot(oc, ray.direction);
    float c = dot(oc, oc) - sphere.radius * sphere.radius;
    float disc = b * b - 4 * a * c;
    if(disc < 0) return -1f;
    return (-b - sqrt(disc)) / 4 * a * c;
}