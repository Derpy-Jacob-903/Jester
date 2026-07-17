shader_type canvas_item;

uniform float stripe_size = 50.0;
uniform float angle_deg = -120.0;

// Your exact CSS colors converted to HSV
const vec3 PALETTE[8] = vec3[](
    vec3(0.975, 0.32, 0.97), // #f8aeb8
    vec3(0.067, 0.30, 0.97), // #f8ccae
    vec3(0.145, 0.30, 0.97), // #f8f1ae
    vec3(0.221, 0.32, 0.78), // #b1c88c
    vec3(0.488, 0.30, 0.97), // #aef8f4
    vec3(0.583, 0.32, 0.97), // #aecff8
    vec3(0.725, 0.32, 0.97), // #c0aef8
    vec3(0.875, 0.32, 0.97)  // #f8aedf
);

varying vec4 modulate_color;

void vertex() {
    modulate_color = COLOR;
}

vec3 hsv2rgb(vec3 c) {
    vec3 rgb = clamp(
        abs(mod(c.x * 6.0 + vec3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0,
        0.0,
        1.0
    );
    rgb = rgb * rgb * (3.0 - 2.0 * rgb);
    return c.z * mix(vec3(1.0), rgb, c.y);
}

void fragment() {
    float a = radians(angle_deg);
    vec2 dir = vec2(cos(a), sin(a));

    // Use screen-space pixels so stripe_size is in actual pixels
    vec2 screen_size = 1.0 / SCREEN_PIXEL_SIZE; // SCREEN_PIXEL_SIZE = 1/size
    vec2 p = SCREEN_UV * screen_size;

    float t = dot(p, dir);
    int idx = int(floor(t / stripe_size)) % 8;
    if (idx < 0) idx += 8;

    vec3 hsv = PALETTE[idx];
    vec3 rgb = hsv2rgb(hsv);

    COLOR = vec4(rgb, 1.0) * modulate_color;
}
