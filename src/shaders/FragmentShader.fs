#version 330 core

#define MAX_STEPS 100
#define SURFACE_DIST 0.01
#define MAX_DIST 100
// Interpolated values from the vertex shaders
// in vec3 fragmentColor;
uniform ivec2 u_resolution;

out vec3 color;

struct Camera {
  vec3 ro; // ray origin
  vec3 rd; // ray direction
};

float sphereSDF(vec3 p, vec4 sphere) {
  return length(p - sphere.xyz) - sphere.w;
}

float sceneSDF(vec3 p) {
  vec4 sphere = vec4(0, 2, -5, 1);
  float dSphere = sphereSDF(p, sphere);
  float dPlane = p.y;
  float dS = min(dSphere, dPlane);
  return dS;
}

float rayMarch(vec3 ro, vec3 rd) {
  float dOrigin = 0.0; // distance from origin
  for (int i = 0; i < MAX_STEPS; i++) {
    vec3 p = ro + dOrigin * rd; // march the point forward in the direction

    // get distance of  closest object in the scene to the point
    float dScene = sceneSDF(p); // distance to scene
    dOrigin += dScene;

    // we have a hit or if ray marches too far
    if (dScene < SURFACE_DIST || dOrigin > MAX_DIST)
      break;
  }
  return dOrigin;
}

vec3 getNormal(vec3 p) {

  vec2 e = vec2(0.01, 0);
  // intuition taking the gradient of the point and points around it
  // this will give a vector perpendicular, which is the normal;
  vec3 N = vec3(sceneSDF(p + e.xyy) - sceneSDF(p - e.xyy),
                sceneSDF(p + e.yxy) - sceneSDF(p - e.yxy),
                sceneSDF(p + e.yyx) - sceneSDF(p - e.yyx));

  return normalize(N);
}

vec3 lightingCalculation(vec3 p) {
  vec3 lightPos = vec3(0, 5, -5);
  vec3 L = normalize(lightPos - p);
  vec3 N = getNormal(p);

  float diffColor = dot(N, L);
  return vec3(diffColor);
}

void main() {
  vec2 uv = (gl_FragCoord.xy - 0.5 * u_resolution.xy) / u_resolution.y;

  Camera camera = Camera(vec3(0, 1, 1), normalize(vec3(uv.x, uv.y, -1)));

  float d = rayMarch(camera.ro, camera.rd); // nearest distance to the scene

  vec3 p = camera.ro + camera.rd * d;

  color = lightingCalculation(p);
  // gl_FragColor = vec4(color, 1.0);
}