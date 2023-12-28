shader_type canvas_item;

uniform vec4 from_color: source_color;
uniform vec4 to_color: source_color;


void fragment() {
	vec4 current_color = texture(TEXTURE, UV);
	
	if(current_color== from_color) {
		COLOR = to_color;
	}else {
		COLOR = current_color;
	}
}