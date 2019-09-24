//
//  RatePopUPDelegate.m
//
//  Created by Osipov Stanislav on 1/12/13.
//
//

#import "RateDialogDelegate.h"
#import "IOSNative.h"

@implementation RateDialogDelegate

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    [IOSNative UnregisterAllertView];
    
    char *result = NULL;
    if( buttonIndex == 0 ) result = "Yes";
    else if( buttonIndex == 1 ) result = "Later";
    else if( buttonIndex == 2 ) result = "No";
    
    UnitySendMessage("NativeIOSListener", "OnRateDialogResult", result);
}

@end