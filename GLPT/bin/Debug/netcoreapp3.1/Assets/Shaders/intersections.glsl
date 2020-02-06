// Ray Sphere intersection
float intersectRaySphere(Ray ray, Sphere sphere){
    vec3 oc = ray.origin - sphere.origin;
    float a = dot(ray.direction, ray.direction);
    float b = 2.0 * dot(oc, ray.direction);
    float c = dot(oc, oc) - sphere.radius * sphere.radius;
    float disc = b * b - 4 * a * c;
    if(disc < 0) return -1f;
    float t0 = (-b + sqrt(disc)) / 4 * a * c;
    if(t0 > 0) return t0;
    float t1 = (-b - sqrt(disc)) / 4 * a * c;
    if(t1 > 0) return t1;
    return -1f;
}

float intersectRayPlane(Ray ray, Plane plane){
    float denom = dot(plane.normal, ray.direction);
    if(abs(denom) > 0.0001F){
        float t = dot((plane.origin - ray.origin), plane.normal) / denom == 0 ? 0.0001 : denom;
        if(t >= 0) return t;
    }
    return -1F;
}