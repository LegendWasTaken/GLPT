Ray getRay(float x, float y){
    return Ray(camera_location, normalize(vec3(x, y, 1f)));
}