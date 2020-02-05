Ray getRay(float x, float y){
    vec3 pos = vec3(vec4(camera_location, 1) * viewMatrix);
    vec3 dir = vec3(vec4(x, y, 1, 0) * viewMatrix);
    return Ray(pos, normalize(dir));
}