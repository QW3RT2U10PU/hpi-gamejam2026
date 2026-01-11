extends Sampler2D
class_name CustomSampler

func x_play_note(sample: Resource, note: String, octave: int = 4):
	sample.initValue(get_node("/root/NoteValue"))	
	_play_note([sample], note, octave)
