// made by someone @ stackoverflow
// http://stackoverflow.com/questions/1117211/how-would-i-tint-an-image-programatically-on-the-iphone

@interface CSTintedImageView : UIView

@property (strong, nonatomic) UIImage * image;
@property (strong, nonatomic) UIColor * tintColor;

- (id)initWithImage:(UIImage *)image;

@end
