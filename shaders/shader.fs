#version 330

in vec2 fragTexCoord;
in vec4 fragColor;
out vec4 finalColor;

// The two textures: the original scene and the bloom texture.
uniform sampler2D sceneTex;
uniform sampler2D bloomTex;
uniform float bloomIntensity;  // Controls the strength of the bloom

void main()
{
    // Sample the scene and bloom textures
    vec4 sceneColor = texture(sceneTex, fragTexCoord);
    vec4 bloomColor = texture(bloomTex, fragTexCoord) * bloomIntensity;
    
    // Combine the two (simple additive blending)
    vec4 combined = sceneColor + bloomColor;
    
    // Quantize with dithering: adjust levels for a posterized look.
    float levels = 8.0;
    
    // A 4x4 Bayer matrix (values normalized in the [0, 1] range)
    float bayer[16] = float[16](
         0.0,    0.5,    0.125,  0.625,
         0.75,   0.25,   0.875,  0.375,
         0.1875, 0.6875, 0.0625, 0.5625,
         0.9375, 0.4375, 0.8125, 0.3125
    );
    
    // Use the fragment's screen coordinates modulo 4 to index the Bayer matrix
    ivec2 pos = ivec2(mod(gl_FragCoord.xy, 4.0));
    int index = pos.y * 4 + pos.x;
    float dither = bayer[index];
    // Remap dither value to a small bias
    float bias = (dither - 0.5) / levels;
    
    // Apply quantization with the bias: scale, round, and re-map to [0, 1]
    vec3 ditheredColor = floor(combined.rgb * levels + bias + 0.5) / (levels - 1.0);
    
    finalColor = vec4(ditheredColor, combined.a);
}
