#include "gl_helper.hpp"

//
#include "camera.hpp"
#include "geometry.hpp"
#include "trackball_ctrl.hpp"

int main() {
    int programID = initGLProgram("Water");

    if (programID == -1) {
        fprintf(stderr, "something went wrong while initializing");
        return programID;
    }

    PerspectiveCamera *camera = new PerspectiveCamera(70.0, 4.0 / 3.0, 0.1, 100.0);
    camera->setPosition(glm::vec3(0, 3, 3));
    //BoxGeometry *box = new BoxGeometry(1.0f, 1.0f, 1.0f);
    Geometry *imagePlane = new Geometry();
    imagePlane->vertices = {
        -1.0f, 1.0f, 0.0f,
        -1.0f, -1.0f, 0.0f,
        1.0f, -1.0f, 0.0f,
        1.0f, 1.0f, 0.0f};

    imagePlane->indices = {
        0, 1, 3,
        3, 1, 2};
    //TrackballControl *trackBallControl = new TrackballControl(window, camera);
    // TODO: implement scene graphs
    //render(camera, box);
    render(camera, imagePlane);
}